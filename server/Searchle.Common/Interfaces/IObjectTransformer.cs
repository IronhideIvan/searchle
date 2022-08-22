namespace Searchle.Common.Interfaces
{
  public interface IObjectTransformer<From, To>
  {
    To Transform(From obj);
  }
}
