using ProjectWasel21.Models;
using ProjectWasel21.Models.ModelsDTO;
using ProjectWasel21.Models.Repositres;
using ProjectWasel21.Repositories;
using ProjectWasel21.Repositres;
using RouteModel = ProjectWasel21.Models.Route;

namespace ProjectWasel21.Services
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
            var checkpoints = await _checkpointRepo.GetAllAsync();

            var considered = checkpoints
                .Where(c =>
                    !request.AvoidCheckpoints.Contains(c.CheckpointId) &&
                    !request.AvoidAreas.Contains(c.Name))
                .ToList();

            double distance = HaversineDistance(
                request.StartLat,
                request.StartLng,
                request.EndLat,
                request.EndLng);

            double avgSpeed = 40;
            double duration = distance / avgSpeed;

            // ✅ FIX: use RouteModel (avoid ASP.NET conflict)
            var route = new RouteModel
            {
                StartLat = request.StartLat,
                StartLng = request.StartLng,
                EndLat = request.EndLat,
                EndLng = request.EndLng,
                EstimatedDistance = (decimal)distance,
                EstimatedDuration = (decimal)duration,
                CreatedAt = DateTime.UtcNow
            };

            await _routeRepo.AddAsync(route);

            var metadata = new
            {
                AvoidedCheckpoints = request.AvoidCheckpoints,
                AvoidedAreas = request.AvoidAreas,
                ConsideredCheckpoints = considered.Select(c => new
                {
                    c.CheckpointId,
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
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians((double)lat1)) *
                Math.Cos(ToRadians((double)lat2)) *
                Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        private double ToRadians(double angle) => angle * Math.PI / 180;
    }
}