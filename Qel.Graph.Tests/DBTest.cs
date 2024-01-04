using Qel.Graph.Dal;

namespace Qel.Graph.Tests;

public class DBTest
{
    [Fact]
    public void DbContextMainTest()
    {
        var db = DbContextMain.CreateContext();
        db.Files.Add(new Dal.Entities.File { 
            Id = 999, 
            IsDeleted = false, 
            Json = "test", 
            Path = "test", 
            CreationDateTime = DateTime.UtcNow,
            ModifyDateTime = DateTime.UtcNow,
        });
        db.SaveChanges();

        var testAdd = db.Files.Where(x => x.Json == "test" && x.Path == "test").ToList();
        Assert.NotEmpty(testAdd);
        Assert.NotNull(testAdd);

        db.Files.Remove(new Dal.Entities.File
        {
            Id = 999,
            IsDeleted = false,
            Json = "test",
            Path = "test",
        });
        db.SaveChanges();

        var testRemove = db.Files.Where(x => x.Json == "test" && x.Path == "test").ToList();
        Assert.NotEmpty(testRemove);
        Assert.NotNull(testRemove);
    }
}