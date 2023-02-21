namespace MovieTicket.Views
{
	public interface IViewRender
	{
		public void Render(string? statusMessage = null, object? model = null);
	}
}
