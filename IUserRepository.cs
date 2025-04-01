public interface IUserRepository
{
    Task<IEnumerable<Folder>> GetUserFoldersAsync(int userId);
    Task<Folder?> DeleteFolderAsync(int folderId);
    Task CreateFolderAsync(Folder folder);
    Folder? GetFolderById(int folderId);
    Task<User?> GetUserByIdAsync(int userId);
    Task UpdateUserProfileAsync(User user);
}
