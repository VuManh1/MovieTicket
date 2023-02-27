namespace SharedLibrary
{
	public class Result
	{
		public bool Success { get; set; }
		public string? Message { get; set; }
		public object? Model { get; set; }

		public static Result OK(object? model = null)
		{
			return new()
			{
				Model = model,
				Success = true
			};
		}

		public static Result Error(string? message = null)
		{
			return new Result
			{
				Success = false,
				Message = message
			};
		}


		public static Result NetworkError()
		{
			return new Result
			{
				Success = false,
				Message = "Something wrong :((, check your network or restart application."
			};
		}
	}
}
