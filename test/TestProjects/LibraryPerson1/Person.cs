using System.ComponentModel.DataAnnotations;

namespace LibraryPerson1;
public class Person
{
    private readonly DateOnly _birthDay;
    
    [StringLength(34)]
    private readonly string _name;

    public string Name => _name;

    private readonly string _surname;
    public string Surname => _surname;

    public Person(DateOnly birthDay, string name, string surname)
    {
        _birthDay = birthDay;
        _name = name;
        _surname = surname;
    }

    public void DoWork()
    {
        OnPersonChanged();
    }

    public event EventHandler PersonChanged;
    protected virtual void OnPersonChanged()
    { 
        if (PersonChanged != null) 
            PersonChanged(this,EventArgs.Empty);
        
        Method();
    }
    
    void Method()
    {
        Func<string,string> inlineFunction = source => 
        {
            if (File.Exists(source))
            {
                _ = File.ReadAllText(source);
            }
            return $"{Name} {Surname} born on {_birthDay.ToShortDateString()}";
        };
        // call the inline function
        Console.WriteLine($"Result {inlineFunction("/etc/hosts")}");
    }
}
