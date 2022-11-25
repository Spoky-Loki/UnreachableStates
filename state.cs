using System;
using System.Collections.Generic;
using System.Text;

namespace UnreachableStates
{
    class State
    {
        String name;
        Boolean final;
        Dictionary<String, List<State>> transitions;

        public State(string name, bool final = false)
        {
            this.name = name;
            this.final = final;
            transitions = new Dictionary<string, List<State>>();
        }

        public String getName() { return name; }

        public Boolean getFinal() { return final; }

        public void setFinal() { final = true; }

        public Dictionary<String, List<State>> getTransitions() { return transitions; }

        public void addTransition(String key, State st) {
            if (transitions.ContainsKey(key))
                transitions[key].Add(st);
            else
                transitions.Add(key, new List<State> { st });
        }

        public List<State> achievableStates()
        {
            List<State> res = new List<State>();

            foreach (var tran in transitions)
            {
                foreach (var state in tran.Value)
                    res.Add(state);
            }

            return res;
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            if (final == true)
                res.Append("+ ");
            else
                res.Append("- ");
            res.Append(name + " ");

            foreach (var tran in transitions)
            {
                foreach (var st in tran.Value)
                    res.Append(tran.Key + ":" + st.getName() + " ");
            }

            return res.ToString();
        }
    }
}
