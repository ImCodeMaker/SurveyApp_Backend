public interface INotificationsServices
{
    Task EnviarNotificacionAsync(string emailDestinatario, string asunto, string mensaje);
    Task SendAdminEmail( string mensaje);
}