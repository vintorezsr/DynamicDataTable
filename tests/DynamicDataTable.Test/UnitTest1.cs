using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace DynamicDataTable.Test
{
    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public class UnitTest1
    {
        [Fact]
        public void TestToDataTableResponse()
        {
            var data = new[]
            {
                new Person { Id = 1, Name = "Person 1", Email = "person1@gmail.com" },
                new Person { Id = 2, Name = "Person 2", Email = "person2@gmail.com" },
                new Person { Id = 3, Name = "Person 3", Email = "person3@gmail.com" },
                new Person { Id = 4, Name = "Person 4", Email = "person4@gmail.com" },
                new Person { Id = 5, Name = "Person 5", Email = "person5@ymail.com" },
                new Person { Id = 6, Name = "Person 6", Email = "person6@hotmail.com" },
                new Person { Id = 7, Name = "Person 7", Email = "person7@yahoo.com" }
            };
            var dataTableRequest = new DataTableRequest
            {
                Draw = 1,
                Start = 0,
                Length = 4,
                Columns = new ColumnCollection()
                {
                    new Column { Data = "id", Name = "Id", Searchable = false, Sortable = true },
                    new Column { Data = "name", Name = "Name", Searchable = true, Sortable = true },
                    new Column { Data = "email", Name = "Email", Searchable = true, Sortable = true },
                },
                SortableColumns = new SortableColumnCollection()
                {
                    new SortableColumn { ColumnIndex = 0, SortDirection = SortDirection.Descending }
                }
            };
            var dataTableResponse = data.ToDataTableResponse(dataTableRequest);
            Assert.NotEqual(0, dataTableResponse.RecordsFiltered);
        }

        [Fact]
        public async Task TestToDataTableResponseWithModelBinder()
        {
            var data = new[]
            {
                new Person { Id = 1, Name = "Person 1", Email = "person1@gmail.com" },
                new Person { Id = 2, Name = "Person 2", Email = "person2@gmail.com" },
                new Person { Id = 3, Name = "Person 3", Email = "person3@gmail.com" },
                new Person { Id = 4, Name = "Person 4", Email = "person4@gmail.com" },
                new Person { Id = 5, Name = "Person 5", Email = "person5@ymail.com" },
                new Person { Id = 6, Name = "Person 6", Email = "person6@hotmail.com" },
                new Person { Id = 7, Name = "Person 7", Email = "person7@yahoo.com" }
            };
            var formCollection = new FormCollection(
                new Dictionary<string, StringValues>()
                {
                    { "draw", new StringValues("1") },
                    { "start", new StringValues("0") },
                    { "length", new StringValues("5") },
                    { "search[value]", new StringValues("") },
                    { "order[0][column]", new StringValues("0") },
                    { "order[0][dir]", new StringValues("desc") },
                    { "columns[0][data]", new StringValues("id") },
                    { "columns[0][name]", new StringValues("Id") },
                    { "columns[0][searchable]", new StringValues("false") },
                    { "columns[0][orderable]", new StringValues("true") },
                    { "columns[0][search][value]", new StringValues("") },
                    { "columns[0][search][regex]", new StringValues("false") },
                }
            );
            var vp = new FormValueProvider(BindingSource.Form, formCollection, CultureInfo.CurrentCulture);

            var modelBinder = new DataTableRequestModelBinder();
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var bindingContext = new DefaultModelBindingContext {
                ModelMetadata = modelMetadataProvider.GetMetadataForType(typeof(Person)),
                ModelName = typeof(Person).Name,
                ModelState = new ModelStateDictionary(),
                ValueProvider = vp,
            };
            await modelBinder.BindModelAsync(bindingContext);
            var dataTableRequest = bindingContext.Result.Model as DataTableRequest;
            var dataTableResponse = data.ToDataTableResponse(dataTableRequest!);
            Assert.NotEqual(0, dataTableResponse.RecordsFiltered);
        }
    }
}