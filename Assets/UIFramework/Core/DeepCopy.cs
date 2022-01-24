using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class DeepCopy
    {
        /// <summary>
        /// ���󿽱�
        /// </summary>
        /// <param name="obj">�����ƶ���</param>
        /// <returns>�¶���</returns>
        public static object CopyObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            System.Object targetDeepCopyObj;
            Type targetType = obj.GetType();
            //ֵ����  
            if (targetType.IsValueType == true)
            {
                targetDeepCopyObj = obj;
            }
            //��������   
            else
            {
                targetDeepCopyObj = Activator.CreateInstance(targetType); //�������ö���   
                System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

                foreach (System.Reflection.MemberInfo member in memberCollection)
                {
                    //�����ֶ�
                    if (member.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        System.Reflection.FieldInfo field = (System.Reflection.FieldInfo) member;
                        System.Object fieldValue = field.GetValue(obj);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(targetDeepCopyObj, CopyObject(fieldValue));
                        }

                    } //��������
                    else if (member.MemberType == System.Reflection.MemberTypes.Property)
                    {
                        System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo) member;

                        MethodInfo info = myProperty.GetSetMethod(false);
                        if (info != null)
                        {
                            try
                            {
                                object propertyValue = myProperty.GetValue(obj, null);
                                if (propertyValue is ICloneable)
                                {
                                    myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
                                }
                                else
                                {
                                    myProperty.SetValue(targetDeepCopyObj, CopyObject(propertyValue), null);
                                }
                            }
                            catch (System.Exception)
                            {

                            }
                        }
                    }
                }
            }

            return targetDeepCopyObj;
        }
       

        public static object Copy(object src)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, src);

            ms.Seek(0, SeekOrigin.Begin);
            object dst = bf.Deserialize(ms);
            ms.Close();
            return dst;
        }
}
}