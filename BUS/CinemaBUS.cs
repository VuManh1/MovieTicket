using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;
using SharedLibrary.Helpers;
using SharedLibrary.Models;

namespace BUS
{
    public class CinemaBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public CinemaBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Cinema cinema)
		{
            cinema.Name = cinema.Name.NormalizeString();

            try
            {
                return _unitOfWork.CinemaRepository.Create(cinema);
            }
            catch
            {
                return Result.NetworkError();
            }
        }

		public Result Delete(Cinema cinema)
		{
			try
			{
				return _unitOfWork.CinemaRepository.Delete(cinema);
			}
			catch
			{
				return Result.NetworkError();
			}
		}

		public List<Cinema> GetAll()
		{
            try
            {
                return _unitOfWork.CinemaRepository.GetAll().ToList();
            }
            catch
            {
                return new List<Cinema>();
            }
        }

        public List<Cinema> GetByCityId(int cityid)
        {
            try
            {
                return _unitOfWork.CinemaRepository.Find($"CityId = {cityid}").ToList();
            }
            catch
            {
                return new List<Cinema>();
            }
        }

        public List<Cinema> Find(string filter)
        {
            try
            {
                return _unitOfWork.CinemaRepository.Find(
                    $" cinemas.name like '%{filter}%' OR" +
                    $" cities.name like '%{filter}%'").ToList();
            }
            catch
            {
                return new List<Cinema>();
            }
        }

		public Cinema? GetById(int id)
		{
            try
            {
                return _unitOfWork.CinemaRepository.GetById(id);
            }
            catch
            {
                return null;
            }
        }

		public Result Update(Cinema entity)
		{
            entity.Name = entity.Name.NormalizeString();

            try
            {
			    return _unitOfWork.CinemaRepository.Update(entity);
            }
            catch
            {
                return Result.NetworkError();
            }
		}

        public List<Hall> GetHalls(Cinema cinema)
        {
            try
            {
                return _unitOfWork.CinemaRepository.GetHalls(cinema).ToList();
            }
            catch
            {
                return new List<Hall>();
            }
        }

        public List<Seat> GetSeats(Hall hall)
        {
            try
            {
                return _unitOfWork.HallRepository.GetSeats(hall).ToList();
            }
            catch
            {
                return new List<Seat>();
            }
        }

        public Result CreateHall(Hall hall)
        {
            try
            {
			    return _unitOfWork.HallRepository.Create(hall);
            }
            catch
            {
                return Result.NetworkError();
            }
        }

        public Result CreateSeat(Seat seat)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                Result result = _unitOfWork.SeatRepository.Create(seat);

                List<Show> shows = _unitOfWork.ShowRepository.Find($"HallId = {seat.Hall.Id}").ToList();

                // Add to showseat table
                foreach (Show show in shows)
                {
                    _unitOfWork.ShowRepository.AddToShowSeat(show, new List<Seat>() { seat });
                }

                _unitOfWork.CommitTransaction();
            }
            catch(Exception e)
            {
                _unitOfWork.RollBack();
                return Result.Error(e.Message);
            }

            return Result.OK();
        }

        public Result DeleteSeat(Seat seat)
        {
            try
            {
                return _unitOfWork.SeatRepository.Delete(seat);
            }
            catch
            {
                return Result.NetworkError();
            }
        }

        public Result UpdateHall(Hall hall)
        {
            try
            {
                return _unitOfWork.HallRepository.Update(hall);
            }
            catch
            {
                return Result.NetworkError();
            }
        }

        public Result DeleteHall(Hall hall)
        {
            try
            {
                return _unitOfWork.HallRepository.Delete(hall);
            }
            catch
            {
                return Result.NetworkError();
            }
        }
    }
}
