using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnreachableStates
{
    class functions
    {
        public static State ContainsStateWithName(String name, List<State> statesList)
        {
            foreach (var state in statesList)
            {
                if (state.getName().Equals(name))
                    return state;
            }

            return null;
        }

        public static List<State> readFromFile(String path)
        {
            List<State> res = new List<State>();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    String[] alphabet = reader.ReadLine().Split(' ');
                    String line;
                    State st;

                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] arr = line.Split(' ');
                        st = ContainsStateWithName(arr[1], res);
                        if (st == null)
                        {
                            st = new State(arr[1], arr[0] == "+");
                            res.Add(st);
                        }
                        else if (arr[0] == "+")
                            st.setFinal();

                        for (int i = 2; i < arr.Length; i++)
                        {
                            String[] temp = arr[i].Split(':');
                            var tempSt = ContainsStateWithName(temp[1], res);
                            if (tempSt == null)
                            {
                                tempSt = new State(temp[1], false);
                                res.Add(tempSt);
                                st.addTransition(temp[0], tempSt);
                            }
                            else
                            {
                                st.addTransition(temp[0], tempSt);
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message.ToString());
            }

            return res;
        }

        public static void createFileWithTable(String path, List<State> listStates)
        {
            try
            {
                if (listStates.Count == 0)
                    throw new Exception("Передан пустой список состояний!");

                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    var transitions = listStates[0].getTransitions();
                    foreach (var key in transitions.Keys)
                        writer.Write(key + " ");
                    writer.WriteLine();

                    foreach (var state in listStates)
                    {
                        writer.WriteLine(state.ToString());
                    }
                }
            }
            catch(Exception e) { Console.WriteLine(e.Message.ToString()); }
        }

        public static void removeUnachievableStates(List<State> listStates)
        {
            if (listStates.Count == 0)
            {
                Console.WriteLine("Передан пустой список состояний!");
                return;
            }

            List<State> tempOld = new List<State>();
            List<State> tempNew = new List<State>();
            tempNew.Add(listStates[0]);
            Boolean flag = true;

            while (flag)
            {
                var distinct = tempNew.Where(o => ContainsStateWithName(o.getName(), tempOld) == null).ToList();
                flag = distinct.Count() != 0;
                if (flag)
                {
                    foreach (var st in distinct)
                    {
                        tempOld.Add(st);

                        foreach (var tran in st.achievableStates())
                        {
                            if (ContainsStateWithName(tran.getName(), tempNew) == null)
                                tempNew.Add(tran);
                        }
                    }
                }
            }

            var unachievableStates = listStates.Where(o => tempNew.Contains(o) == false).ToList();

            foreach (var st in unachievableStates)
                listStates.Remove(st);
        }
    }
}
