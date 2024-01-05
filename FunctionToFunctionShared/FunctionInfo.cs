using System.ComponentModel.DataAnnotations;

namespace FunctionToFunctionShared;
public class FunctionInfo
{
    [Required]
    public string Name { get; set; }
    [Required]
    public int Counter { get; set; }
}
