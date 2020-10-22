using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DTDecision : DTNode {

    private DTCall selector;

    private Dictionary<object, DTNode> links;

    public DTDecision(DTCall selector) {
        this.selector = selector;
        links = new Dictionary<object, DTNode>();
    }
    
    public DTAction walk() {
        object o = selector();
        return links.ContainsKey(o) ? links[o].walk() : null;
    }

    public void addLink(object key, DTNode value) {
        links.Add(key,value);
    }
    
}
