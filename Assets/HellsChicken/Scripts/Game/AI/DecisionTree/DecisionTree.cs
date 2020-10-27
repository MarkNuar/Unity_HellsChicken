using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree {

    public DTNode root;

    public DecisionTree(DTNode root) {
        this.root = root;
    }

    public object start() {
        DTAction result = root.walk();
        if (result != null) 
            return result.action();
        return null;
    }

}
