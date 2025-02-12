namespace DrugsMicroservice.Application.DTOs;

public class DrugUpdateDTO
{
    public string Name { get; set; }
    public string Manufacturer { get; set; }
    public decimal Price { get; set; }
    
    public List<String> Substances { get; set; }
}