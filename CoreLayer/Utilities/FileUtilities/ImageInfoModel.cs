namespace CoreLayer.Utilities.FileUtilities;

public class ImageInfoModel
{
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsBase64 { get; set; }
    public string Base64 { get; set; }
}
