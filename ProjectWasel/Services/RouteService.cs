using ProjectWasel.Models;
using ProjectWasel.Models.ModelsDTO;
using ProjectWasel.Repositories;
using ProjectWasel.Repositres;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Route = ProjectWasel.Models.Route;

namespace ProjectWasel.Services
{
    public class RouteService
    {
        private readonly IRepository<Checkpoint> _checkpointRepo;
        private readonly IRouteRepository _routeRepo;

        public RouteService(IRepository<Checkpoint> checkpointRepo, IRouteRepository routeRepo)
        {
            _checkpointRepo = checkpointRepo;
            _routeRepo = routeRepo;
        }

        public async Task<RouteResponseDTO> EstimateRoute(RouteRequestDTO request)
        {
            // استخدام Raw SQL لجلب كل الـCheckpoints
            var checkpoints = await _checkpointRepo.GetAllAsync();

            var considered = checkpoints
                .Where(c =>
                    !request.AvoidCheckpoints.Contains(c.CheckpointId) &&
                    !request.AvoidAreas.Contains(c.Name))
                .ToList();

            double distance = HaversineDistance(request.StartLat, request.StartLng, request.EndLat, request.EndLng);

            double avgSpeed = 40; // km/h
            double duration = distance / avgSpeed;

            // إنشاء Route
            var route = new Route
            {
                StartLat = request.StartLat,
                StartLng = request.StartLng,
                EndLat = request.EndLat,
                EndLng = request.EndLng,
                EstimatedDistance = (decimal)distance,
                EstimatedDuration = (decimal)duration
            };

            // حفظ Route باستخدام Raw SQL
            await _routeRepo.AddAsync(route);

            // إذا بدك، تقدر تستبدل AddAsync بدالة Raw SQL لاحقاً
            // await _routeRepo.AddRawAsync(route); // لو ضفتها بالـRepository

            var metadata = new
            {
                AvoidedCheckpoints = request.AvoidCheckpoints,
                AvoidedAreas = request.AvoidAreas,
                ConsideredCheckpoints = considered.Select(c => new
                {
                    c.Name,
                    c.Latitude,
                    c.Longitude
                }),
                EstimatedSpeedKmh = avgSpeed
            };

            return new RouteResponseDTO
            {
                EstimatedDistanceKm = distance,
                EstimatedDurationHrs = duration,
                Metadata = metadata
            };
        }

        private double HaversineDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            double R = 6371;
            double dLat = ToRadians((double)(lat2 - lat1));
            double dLon = ToRadians((double)(lon2 - lon1));

            double a =
                System.Math.Sin(dLat / 2) * System.Math.Sin(dLat / 2) +
                System.Math.Cos(ToRadians((double)lat1)) *
                System.Math.Cos(ToRadians((double)lat2)) *
                System.Math.Sin(dLon / 2) *
                System.Math.Sin(dLon / 2);

            double c = 2 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a));

            return R * c;
        }

        private double ToRadians(double angle) => angle * System.Math.PI / 180;
    }
}