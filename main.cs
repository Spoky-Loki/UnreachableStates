namespace UnreachableStates
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var states = functions.readFromFile("NKA.txt");

            functions.removeUnachievableStates(states);

            foreach(var st in states)
                System.Console.WriteLine(st.ToString());

            functions.createFileWithTable("NewNKA.txt", states);
        }
    }
}
