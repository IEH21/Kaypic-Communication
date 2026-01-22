using System.ComponentModel.DataAnnotations;

public class Vote
{
    [Key] public int Id { get; set; }
    public int SondageId { get; set; }
    public int OptionId { get; set; }
    public string UtilisateurId { get; set; }
}