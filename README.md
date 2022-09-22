<h1 align="center">
  RandomSerializer - this project is a test task.
</h1>
<h3>Description:</h3>
<b>Create a program which will execute the next steps:</b>

1. Create collection of randomly generated objects in memory by provided models, number of ofjects 10000;
2. Serialyze it to JSON format;
3. Write the serialization result to the current user desktop directory, the text file name should be "Persons.json";
4. Clear the in memory collection;
5. Read objects from file;
6. Display in console persons count, persons credit card count, the average value of child age.

Use POSIX format for dates.<br>
Use lowerCamelCase JSON notation in result file.

<h3>Data models:</h3>
<pre><code>class Person
{
	public Int32 Id { get; set; }
	public Guid TransportId { get; set; }
	public String FirstName { get; set; }
	public String LastName { get; set; }
	public Int32 SequenceId { get; set; }
	public String[] CreditCardNumbers { get; set; }
	public Int32 Age { get; set; }
	public String[] Phones { get; set; }
	public Int64 BirthDate { get; set; }
	public Double Salary { get; set; }
	public Boolean IsMarred { get; set; }
	public Gender Gender { get; set; }
	public Child[] Children { get; set; }	
}</code></pre>
<pre><code>class Child
{
	public Int32 Id { get; set; }
	public String FirstName { get; set; }
	public String LastName { get; set; }
	public Int64 BirthDate { get; set; }
	public Gender Gender { get; set; }
}</code></pre>
<pre><code>enum Gender
{
	Male,
	Female
}</code></pre>
