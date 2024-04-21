using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    private Human human;
    private Work builder;//test
    

    private void Start()
    {
        
        human = GetComponent<Human>();
        if(human.work !=null){
        builder = human.work;}
        InvokeRepeating("DecideAndAct", 2.0f, 10.0f); // Каждые 5 секунд принимаем решение
    }

     private bool ShouldGoHome()
    {
        // Возвращаем true, если наступила ночь и у персонажа есть дом
        return SimpleDayNightCycle.isNight && human.home != null;
    }

    void DecideAndAct()
    {
        human.CheckForHome();
        if(builder ==null){
        human.TryRequestWork();
         builder = human.work;
        }

        if (human.Energy < 300)
        {
            human.Eat(); // Если энергия меньше 30, персонаж должен есть
        }


        if (ShouldGoHome()) // Проверяем, пора ли идти домой
        {
            human.GoHome(); // Идем домой, если наступила ночь и есть дом
        }
        else if( !SimpleDayNightCycle.isNight && human.home !=null)
        {
            human.MakeVisible();
        }
        

        if(!ShouldGoHome() && builder == null)
        {
          // Debug.Log("Wander");
            human.Wander(); // Иначе продолжаем гулять
        }
       else if (!ShouldGoHome() && human.work.GetType() == typeof(Farmer))
{
    // Приведение типа 'work' к типу 'Farmer' для доступа к свойству 'farm'
    Farmer farmerWork = human.work as Farmer;
    if (farmerWork != null && farmerWork.farm != null)
    {
        // Если работа является фермерством и ферма назначена, выполняем работу
        farmerWork.DoWork(0);
    }
    else
    {
        // В противном случае заставляем человека блуждать
        human.Wander();
        human.TryRequestWorkPlace();
    }
}else if (!ShouldGoHome() && (human.work.GetType() == typeof(Builder)||human.work.GetType() == typeof(Loafer) )){human.Wander();}
        
    }

        private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.B) )
        {
            Debug.Log("B");
            // Проверяем, есть ли у персонажа профессия строителя и может ли он строить
            if (builder != null )
            {
                if( human.work.GetType() == typeof(Builder))
                builder.DoWork(2); // Попросим строителя выполнить свою работу
            }
        }

         if (Input.GetKeyDown(KeyCode.V) )
        {
            Debug.Log("V");
            // Проверяем, есть ли у персонажа профессия строителя и может ли он строить
            if (builder != null)
            {
                if( human.work.GetType() == typeof(Builder))
                builder.DoWork(5); // Попросим строителя выполнить свою работу
            }
        }
          if (Input.GetKeyDown(KeyCode.N) )
        {
            Debug.Log("N");
            // Проверяем, есть ли у персонажа профессия строителя и может ли он строить
            if (builder != null)
            {
                if( human.work.GetType() == typeof(Builder))
                builder.DoWork(0); // Попросим строителя выполнить свою работу
            }
        }

         if (Input.GetKeyDown(KeyCode.C) )
        {
            Debug.Log("C");
            // Проверяем, есть ли у персонажа профессия строителя и может ли он строить
            if (builder != null)
            {
                if( human.work.GetType() == typeof(Farmer))
                builder.DoWork(0); // Попросим строителя выполнить свою работу
            }
        }*/
    }
}
