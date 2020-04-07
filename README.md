# NET Framework Pagination

[![latest version](https://img.shields.io/nuget/v/Luilliarcec.Pagination)](https://www.nuget.org/packages/Luilliarcec.Pagination) 
[![downloads](https://img.shields.io/nuget/dt/Luilliarcec.Pagination)](https://www.nuget.org/packages/Luilliarcec.Pagination)

Allows paging of data from an IOrderedQueryable object.

## Installation

You can install the package via nuget:

```bash
dotnet add package Luilliarcec.Pagination
```

## Usage

Whether you use `Code First` or if you use `Database First`, implement the `Interface` exposed in the Package as `IPaginable` in your Models or Entities.

```csharp
using Luilliarcec.Pagination;
// ...

namespace Entities.Models
{
    public class User : IPaginable
    {
        // ...
    }
}
```

Once this is done, in your data layer, where you access the database to obtain the records, you can use the following example to make your queries.

```csharp
using Luilliarcec.Pagination;
// ...

namespace Data.DataAccessObject
{
    public class UsersDAO
    {
        // ...

        public IDictionary<string, object> GetUsers(string option, int current_page, string searched_phrase)
        {
            var query = _context.Providers.Where(
                model =>
                    model.Name.Contains(searched_phrase) ||
                    model.Email.Contains(searched_phrase)
                ).OrderBy(mod => mod.Name);

            return Pagination.Paginate(query, option, current_page, 10);
        }
    }
}
```

In this way you can make queries, order your records by the field you want, etc. However, remember that the `Paging function` receives an object of type `IOrderedQueryable`, so it is vital that your query is `ordered` before paging it.

The parameters that the paging function receives are:

| Parameters | Type | Description |
| -- | -- | -- |
| query | `IOrderedQueryable` | Query on which the pagination will be applied |
| option | `string` | Option or type of pagination that can only be, `first`, `next`, `previous`, `last`, `current` |
| current_page | `int` | Receive the current page you are working on |
| limit | `int` | Limits of records per page |

This function will return a `dictionary of <string, object>`, in which we will find the following values.

| Keys | Values | Description |
| -- | -- | -- |
| total | `int` | Returns total records |
| per_page | `int` | Returns records per page |
| current_page | `int` | Returns current page you are working on |
| last_page | `int` | Returns the last page or total pages |
| data | `IReadOnlyList<IPaginable>` | Returns paged records in a read-only list

In your view you can manage yourself in the following way.:

```csharp
using Luilliarcec.Pagination;
// ...

namespace Views.Users
{
    public partial class FormManageUsers : Form
    {
        private int _current_page;

        // ...

        private void ListData(string option = "first", string searched_phrase = "", int current_page = 1)
        {
            var data = _dao.GetUsers(option, current_page, searched_phrase);

            var items = (IReadOnlyCollection<IPaginable>)data["data"];
            // OR // var items = (IReadOnlyCollection<object>)data["data"];

            DgvProviders.Rows.Clear();

            foreach (User item in items)
            {
                DgvProviders.Rows.Add(
                    item.Id,
                    item.Name,
                    item.Email,
                    item.Address
                );
            }

            _current_page = (int)data["current_page"];

            LbCurrentPage.Text = _current_page.ToString();
            LbTotalPages.Text = ((int)data["last_page"]).ToString();
        }

        private void BtnFirstPage_Click(object sender, EventArgs e)
        {
            ListData("first", TxtSearch.Text);
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            ListData("next", TxtSearch.Text, _current_page);
        }

        private void BtnPreviousPage_Click(object sender, EventArgs e)
        {
            ListData("previous", TxtSearch.Text, _current_page);
        }

        private void BtnLastPage_Click(object sender, EventArgs e)
        {
            ListData("last", TxtSearch.Text);
        }
    }
}
```

Remember not to manipulate the value of current_page because the package works for you to increase and decrease its value, in addition to validating that it is not out of range.

Follow these tips and have a happy code.

### Security

If you discover any security related issues, please email luilliarcec@gmail.com instead of using the issue tracker.

## Credits

- [Luis Andrés Arce C.](https://github.com/luilliarcec)
- [All Contributors](../../contributors)

## License

The MIT License (MIT). Please see [License File](LICENSE.md) for more information.
