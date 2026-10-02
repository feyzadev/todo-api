var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var todos = new List<TodoItem>
{
    new TodoItem(1, "Bootcamp başvurusu yap", false),
    new TodoItem(2, "GitHub'a proje yükle", false)
};
int nextId = 3;

app.MapGet("/api/todos", () => todos);

app.MapGet("/api/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    return todo is not null ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/api/todos", (TodoItem item) =>
{
    var newItem = new TodoItem(nextId++, item.Title, item.IsCompleted);
    todos.Add(newItem);
    return Results.Created($"/api/todos/{newItem.Id}", newItem);
});

app.MapPut("/api/todos/{id}", (int id, TodoItem updated) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    todo.Title = updated.Title;
    todo.IsCompleted = updated.IsCompleted;
    return Results.NoContent();
});

app.MapDelete("/api/todos/{id}", (int id) =>
{
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo is null) return Results.NotFound();

    todos.Remove(todo);
    return Results.NoContent();
});

app.Run();

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }

    public TodoItem() { }
    public TodoItem(int id, string title, bool isCompleted)
    {
        Id = id;
        Title = title;
        IsCompleted = isCompleted;
    }
}