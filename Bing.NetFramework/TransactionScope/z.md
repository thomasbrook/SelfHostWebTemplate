### 除非您在事务范围内打开连接，否则连接不会自动加入事务范围。
#### Automatic Enlistment
```
  using (var scope = new TransactionScope())
      {
        con.Open();                                
         //update/delete/insert commands here
      }
```

#### Manual Enlistment
```  
    con.Open();
    using (var scope = new TransactionScope())
    {
       con.EnlistTransaction(Transaction.Current);  
       //update/delte/insert statements here
    }
```