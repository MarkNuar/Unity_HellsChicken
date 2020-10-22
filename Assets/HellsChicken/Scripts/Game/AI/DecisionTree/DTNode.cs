using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DTNode { 
    DTAction walk();
}

public delegate object DTCall ();