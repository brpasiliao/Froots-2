using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaggable {
    bool isTagged { get; set; }

    void GetTagged();
}
