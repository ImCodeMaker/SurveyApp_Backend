public sealed class UserSessionManager
{
    private static UserSessionManager? _instance;
    private static readonly object _lock = new object();
    
    
    private readonly Dictionary<int, bool> _activeSessions = new Dictionary<int, bool>();
    
    
    private int? _currentAdminId = null;

    private UserSessionManager() { }

    public static UserSessionManager Instance
    {
        get
        {
            lock (_lock)
            {
                return _instance ??= new UserSessionManager();
            }
        }
    }

    public bool TryLogin(User user)
    {
        lock (_lock)
        {
            
            if (_activeSessions.ContainsKey(user.Id))
                return false;

            
            if (user.IsAdmin)
            {
                if (_currentAdminId != null)
                    return false;
                
                _currentAdminId = user.Id;
            }

            _activeSessions.Add(user.Id, user.IsAdmin);
            return true;
        }
    }

    public bool IsUserLoggedIn(int userId)
    {
        lock (_lock)
        {
            return _activeSessions.ContainsKey(userId);
        }
    }

    public bool Logout(int userId)
    {
        lock (_lock)
        {
            if (!_activeSessions.TryGetValue(userId, out bool isAdmin))
                return false;

            _activeSessions.Remove(userId);
            
            if (isAdmin && _currentAdminId == userId)
            {
                _currentAdminId = null;
            }

            return true;
        }
    }
}