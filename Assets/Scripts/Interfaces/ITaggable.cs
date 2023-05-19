using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaggable {
    bool IsTagged { get; set; }

    void GetTagged();
}
