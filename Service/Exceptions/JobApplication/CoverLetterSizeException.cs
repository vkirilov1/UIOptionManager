namespace Service.Exceptions.JobApplication
{
    public class CoverLetterSizeException() : Exception($"Cover Letter must be at least 200 characters!")
    {
    }
}
