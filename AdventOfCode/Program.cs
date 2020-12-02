using AdventOfCode.Dec2;
using AdventUtilities.Dec1;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventUtilities
{
    class Program
    {
        static void Main(string[] args)
        {
            var methods = Assembly.GetExecutingAssembly().GetModules()
                .SelectMany(m => m.GetTypes())
                .SelectMany(t => t.GetMethods())
                .Where(m => m.CustomAttributes.Any(c => c.AttributeType == typeof(AdventAttribute)))
                .OrderByDescending(m => (m.GetCustomAttributes(typeof(AdventAttribute), true).First() as AdventAttribute).Name);

            foreach(var method in methods)
            {
                try
                {
                    var attribute = method.GetCustomAttributes(typeof(AdventAttribute), true).First() as AdventAttribute;
                    if(method.GetParameters().Length != 1 || method.GetParameters()[0].ParameterType != typeof(TextReader))
                    {
                        throw new Exception("Method has wrong parameter");
                    }
                    if(method.ReturnType != typeof(string))
                    {
                        throw new Exception("Returntype is not string");
                    }
                    if (method.DeclaringType.GetConstructor(new Type[0]) == null)
                    {
                        throw new Exception("No empty constructor");
                    }
                    using var input = new StreamReader(new FileStream(attribute.Filename, FileMode.Open));
                    var clazz = method.DeclaringType.GetConstructor(new Type[0]).Invoke(null);
                    var retVal = method.Invoke(clazz, new object[] { input }) as string;
                    Console.Out.WriteLine($"Result for {attribute.Name} is {retVal}");

                } catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            //var reader = Console.In;
            //var result = args[0] switch
            //{
            //    "dec1-1" => SalaryReport.FindMultOfTargetSum(reader),
            //    "dec1-2" => SalaryReport.FindMultOfThriceTargetSum(reader),
            //    "dec2-1" => PasswordPhilosophy.ValidPasswords(reader),
            //    "dec2-2" => PasswordPhilosophy.ValidPasswordsPart2(reader)

            //};
            //Console.Out.WriteLine(result);
        }
    }
}
