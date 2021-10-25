using Corpo.Skeleton.Features.Categories;

namespace Corpo.Skeleton.Tests.Features.Categories;

public class CategoryListTest : FeatureTest
{
    [Test]
    public async Task Can_list_categories()
    {
        // arrange
        var categories = _.MakeManySaving<Category>();
            
        // act
        var result = await _.SendAsync(new CategoryList.Query());
            
        // assert
        result.Items.ShouldCount(categories.Count());
    }
}