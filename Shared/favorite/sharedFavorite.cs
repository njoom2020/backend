namespace Shared.FavoriteDTO;

public class FavoriteDTO
{
    public FavoriteDTO(int id, int userId, int itemId)
    {
        this.Id = id;
        this.UserId = userId;
        this.ItemId = itemId;
    }
    public int Id { get; set; } = -1;
    public int UserId { get; set; }
    public int ItemId { get; set; }


}
