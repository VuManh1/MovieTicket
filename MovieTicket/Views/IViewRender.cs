namespace MovieTicket.Views
{
	public interface IViewRender
	{
		public void Render(object? model = null, string? previousView = null, string? statusMessage = null);
	}
}
