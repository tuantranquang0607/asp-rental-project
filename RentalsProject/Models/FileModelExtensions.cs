namespace RentalsProject.Models
{
    public static class FileModelExtensions
    {
        public static FileModel? ConvertToModel( this IFormFile upload, int? id = null )
        {
            if (upload == null || upload.Length <= 0)
            {
                return null;
            }

            var model = new FileModel()
            {
                FileName = Path.GetFileName( upload.FileName ),
                ContentType = upload.ContentType,
            };

            model.Content = upload.GetContent();

            if (id != null)
            {
                model.Id = id.Value;
            }

            return model;
        }


        public static byte[] GetContent( this IFormFile upload )
        {
            using (var data = new MemoryStream())
            {
                upload.CopyTo( data );

                var dataArray = data.ToArray();

                return dataArray;
            }
        }
    }
}
