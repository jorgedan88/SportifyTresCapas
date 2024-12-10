namespace Sportify_Back.Models;
public class ClassConcurrenceReport
{
    public int ClassId { get; set; }
    public string ClassName { get; set; }
    public string ProfesorName { get; set; }
    public string ActivityName { get; set; }
    public int UserCount { get; set; }
}
