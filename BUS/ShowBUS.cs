using SharedLibrary;
using SharedLibrary.DTO;
using DAL.UnitOfWork;

namespace BUS
{
    public class ShowBUS
    {
        private readonly IUnitOfWork _unitOfWork;

		public ShowBUS(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public Result Create(Show show)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				Result result = _unitOfWork.ShowRepository.Create(show);

				List<Seat> seats = _unitOfWork.HallRepository.GetSeats(show.Hall).ToList();

				if(seats.Count > 0) _unitOfWork.ShowRepository.AddToShowSeat(show, seats);

				_unitOfWork.CommitTransaction();
			}
			catch(Exception e)
			{
				_unitOfWork.RollBack();
				return Result.Error(e.Message);
			}

			return Result.OK();
		}

		public Result Delete(Show show)
		{
			try
			{
				return _unitOfWork.ShowRepository.Delete(show);
			}
			catch
			{
				return Result.NetworkError();
			}
		}

		public List<Show> GetAll()
		{
			try
			{
				return _unitOfWork.ShowRepository.GetAll().ToList();
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
				return new List<Show>();
			}
		}

        public List<Show> Find(string filter)
        {
            try
            {
                return _unitOfWork.ShowRepository.Find(filter).ToList();
            }
            catch
            {
                return new List<Show>();
            }
        }

        public List<ShowSeat> GetShowSeats(Show show)
        {
            try
            {
                return _unitOfWork.ShowRepository.GetShowSeats(show).ToList();
            }
            catch
            {
                return new List<ShowSeat>();
            }
        }

		public Show? GetById(int id)
		{
			try
			{
				return _unitOfWork.ShowRepository.GetById(id);
			}
			catch
			{
				return null;
			}
		}

		public Result Update(Show entity)
		{
			return _unitOfWork.ShowRepository.Update(entity);
		}
	}
}
