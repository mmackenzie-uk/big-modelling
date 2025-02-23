// See https://aka.ms/new-console-template for more information
// Program structure from https://www.youtube.com/watch?v=IFoxBHnkyrw

// record vs class --> records are immutable which means their properties can 
// not be changed (throws an exception at compile time)



Console.WriteLine("Hello, World!");

Reminder reminder1 = new Reminder(Guid.NewGuid(), "Subscribe", DateTime.Today);
Reminder reminder2 = new Reminder(Guid.NewGuid(), "Subscribe", DateTime.Today);

User user = new User("You watched", SubscriptionType.Free);

user.SetReminder(reminder1);
user.SetReminder(reminder2);
record Reminder(
    Guid Id,
    string Title, 
    DateTime DateTime,
    string Description = "default description",
    bool IsDimissed = false
);
record User(string Name, SubscriptionType SubscriptionType) 
{
    // public List<Reminder> Reminders{ get; } = []; --> incorrect. adds latency

    public Dictionary<DateOnly, int> DateToRemindersMap { get; } = [];

    private readonly List<Guid> _reminderIds = [];

    internal void SetReminder(Reminder reminder)
    {
       
        var maxRemindersPerDay = SubscriptionType switch
        {
            SubscriptionType.Free => 1,
            SubscriptionType.Starter => 2,
            SubscriptionType.Pro => 5,
            _ => throw new InvalidOperationException(),
        };

        var day = DateOnly.FromDateTime(reminder.DateTime);        
        
        if (DateToRemindersMap.TryGetValue(day, out var remindersCount)
            && remindersCount >= maxRemindersPerDay) {
            throw new Exception("Too many reminders");
        }
        _reminderIds.Add(reminder.Id);
        DateToRemindersMap[day] = ++remindersCount;
    }
}

enum SubscriptionType
{
    Free,
    Starter,
    Pro
}
