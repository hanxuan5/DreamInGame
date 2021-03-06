using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ObjectInfo
{
    private string image_;
    private string position_;
    private string message_;

    public ObjectInfo()
    {
        image_ = "";
        position_ = "";
        message_ = "";
    }

    public ObjectInfo(string image, string position, string message)
    {
        image_ = image;
        position_ = position;
        message_ = message;
    }

    public string GetImage()
    {
        return image_;
    }

    public string GetPosition()
    {
        return position_;
    }

    public string GetMessage()
    {
        return message_;
    }

    public void SetImage(string image)
    {
        image_ = image;
    }
    
    public void SetPosition(Vector3 position)
    {
        position_ = "[" + position.x + "," + position.y + "," + position.z + "]";
    }

    public void SetMessage(string message)
    {
        message_ = message;
    }
    
    /// <summary>
    /// Convert to json data
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return string.Format("\"image_link\": \"{0}\",\"position\": \"{1}\",\"message\": \"{2}\"", image_, position_, message_);
    }
}
