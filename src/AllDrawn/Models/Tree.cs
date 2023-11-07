namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;

public class Tree
{
	public string Id { get; set; }

	public string Parent { get; set; }

	public List<Tree> Children { get; set; }

	public int Total { get; set; }
}
