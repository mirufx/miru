using Corpo.Skeleton.Features.Categories;

namespace Corpo.Skeleton.Tests.Features.Categories;

public class CategoryEditTest : FeatureTest
{
    [Test]
    public async Task Can_edit_category()
    {
        // arrange
        var category = _.MakeSaving<Category>();
        var command = _.Make<CategoryEdit.Command>(m => m.Id = category.Id);

        // act
        var result = await _.SendAsync(command);

        // assert
        var saved = _.Db(db => db.Categories.First());
        saved.Name.ShouldBe(command.Name);
    }

    public class Validations : ValidationTest<CategoryEdit.Command>
    {
        [Test]
        public void Name_is_required()
        {
            ShouldBeValid(Request, m => m.Name, Request.Name);
            
            ShouldBeInvalid(Request, m => m.Name, string.Empty);
        }
    }
}