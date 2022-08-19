namespace Searchle.Common.Interfaces
{
  public interface IObjectMapper<From, To>
  {
    To Transform(From obj);
  }
}
