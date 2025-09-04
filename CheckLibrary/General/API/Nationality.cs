namespace CheckLibrary.Data.API
{
   public record class NameCountry(
       String? Common = null,
       String? Official = null
   );
    /// <summary>Classe que recebe dados da API https://restcountries.com/v3.1/</summary>
    /// <param name="Name"></param>
    /// <param name="Flag"></param>
    public record class Country(
    NameCountry? Name = null,
    String? Flag = null
    );
}