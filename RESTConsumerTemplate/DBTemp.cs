namespace RESTConsumerTemplate
{
    public class DBTemp
    {
        // Propertis og Data field
        // Mine Propertis har to auto accessors get og set som man også kan kalde read-write property
        // Get = skal returnerne
        // Set = Mener om en metod som returtype er void
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Parameterized Constructor 
        //Har ingen returtype
        //Dem der er i () hedder Arguments
        public DBTemp(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            
        }

        //Tøm Constructor
        public DBTemp() { }

        //ToString
        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}";
        }
    }
}
