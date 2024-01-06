using Qel.Graph.Dal;

namespace Qel.Graph.Tests;

public class DBTest
{
    [Fact]
    public void DbContextMainTestAdd()
    {
        using var db = DbContextMain.CreateContext();
        
        db.Files.Add(new Dal.Entities.File { 
            IsDeleted = false, 
            Text = "test", 
            Edges = "test", 
            EdgesColor = "test",
            NodesColor = "test",
            CreationDateTime = DateTime.UtcNow,
            ModifyDateTime = DateTime.UtcNow,
        });
        db.SaveChanges();

        var testAdd = db.Files.Where(x => x.Text == "test" && x.Edges == "test").ToList();
        Assert.NotEmpty(testAdd);
        Assert.NotNull(testAdd);
    }

    [Fact]
    public void DbContextMainTestRemove()
    {
        using var db = DbContextMain.CreateContext();
        var testRemove = db.Files.Where(x => x.Text == "test" && x.Edges == "test").ToList();
        db.Files.RemoveRange(testRemove);
        db.SaveChanges();

        var testAfterRemove = db.Files.Where(x => x.Text == "test" && x.Edges == "test").ToList();
        //Assert.Empty(testAfterRemove);
        testAfterRemove.ForEach(x => Assert.True(x.IsDeleted));
    }
}