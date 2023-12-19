using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;
using PiranhaCMS.ContentTypes.Blocks.Base;
using PiranhaCMS.ContentTypes.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PiranhaCMS.ContentTypes.Blocks;

[BlockGroupType(
	Name = "Services Block Group",
	Display = BlockDisplayMode.Vertical,
	Category = Global.TeasersCategory)]
[BlockItemType(Type = typeof(TeaserBlock))]
public class ServicesBlockGroup : BlockGroupBase, ISearchable
{
	[Field(
		Title = "Title",
		Description = "This is block main heading")]
	[Required(ErrorMessage = $"{nameof(Title)}: required!")]
	public StringField Title { get; set; }

	[Field(
		Title = "Heading",
		Description = "This is block sub heading")]
	[Required(ErrorMessage = $"{nameof(Heading)}: required!")]
	public StringField Heading { get; set; }

	#region ISearchable implementation

	public string GetIndexedContent()
	{
		var sb = new StringBuilder();

		sb.AppendLine(Heading.GetIndexedContent());
		sb.AppendLine(Title.GetIndexedContent());

		return sb.ToString();
	}

	#endregion
}
