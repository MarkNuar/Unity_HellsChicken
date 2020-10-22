using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTAction : DTNode {

    public DTCall action;
    
    public DTAction (DTCall action) {
        this.action = action;
    }

    public DTAction walk() {
        return this;
    }
}
