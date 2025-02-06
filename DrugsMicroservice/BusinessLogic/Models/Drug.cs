namespace DrugsMicroservice.BusinessLogic.Models;

public class Drug
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Manufacturer { get; set; }
    public decimal Price { get; set; }
    public List<Substance> Substances { get; set; }
}