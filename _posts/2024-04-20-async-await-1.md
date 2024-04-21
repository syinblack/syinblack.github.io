---
title: async await 1
date: 2024-04-20 08:00:00 +09:00
categories: [비동기 프로그래밍, async await]
tags:
  [
    async,
    await,
    비동기,
    비동기 작업이 Task 를 반환해야 하는 이유,
  ]
---

- async와 await 두 가지 키워드를 사용해 비동기 메서드를 작성할 수 있다.

- 비동기 메서드를 만들기 위해서 우선 async 키워드를 메서드 선언에 추가해야 하고, await 비동기 작업을 대기한다.
- 예시로 이해해보자.
```cs
async Task PrintNumberAsync()
{
    // '비동기적'으로 1초 대기한다.
    await Task.Delay(1000);

    Console.WriteLine(5);
}
```

- Task.Delay()를 비롯한 모든 Task 기반 비동기 작업은 Task/Task<TResult>를 반환한다.
    - 사진

- Task(비동기 작업)는 8가지 상태를 갖는다.
    - Created, WaitingForActivation, Running, WaitingForChildrenToComplete, RanToCompletion, Canceled, Faulted

- 이중 Running, Canceled, Faulted, RanToCompletion이 중요한데 
    비동기 작업이 진행 중일 경우 Running,
    비동기 작업이 취소된 경우 Canceled,
    비동기 작업이 실패한 경우는 Faulted,
    비동기 작업이 성공한 경우는 RanToCompletion 상태를 갖는다.

- 비동기 작업을 await으로 대기하면 컴파일러는 상태 머신으로 비동기 작업의 상태를 추적한다. 비동기 작업 완료 시까지 대기하다가 완료(성공/실패/취소)되면 다음 코드를 실행하는 것이다.
    - async 메서드를 선언하면 컴파일러는 해당 메서드의 상태 머신을 생성한다.

- 비동기 작업의 상태를 생각하며, 다시 예제를 살펴보자.
### 예제
```cs
async Task PrintNumberAsync()
{
    // Task.Delay() 메서드는 비동기 작업으로, Task를 반환한다.
    var task = Task.Delay(1000);
    // 반환된 Task의 상태는 1초동안 Running 이고, 그 후 RanToCompletion으로 변한다.
    await task;

    // task가 완료되면 그 다음 코드가 이어서 실행된다.
    Console.WriteLine(5);
}
```

### 비동기 작업이 Task 를 반환해야 하는 이유
- 비동기 작업이 Task를 반환해야 하는 이유는 작업을 호출한 쪽에서 상태를 추적하기 위함이다.

- 이런 이유로 async void는 절대 사용하지 말아야 한다. 비동기 메서드가 void를 반환하면, 호출한 쪽에서는 해당 작업의 상태를 받지 못하고 그 말은 어떠한 결과도 받을 수 없다는 뜻이다. 
    - 언제 완료됐는지도 모른다(= 대기할 수 없다).
    - 심지어 예외가 발생하더라도 알 수 없다(= 예외 처리 불가).

> async void 비동기 메서드는 외부에서 호출만 가능하고 대기할 수 없으므로 무책임한 비동기 메서드이다.
    (WPF, WinForm, MAUI 등 GUI 프로그램에서 자동 생성되는 이벤트 핸들러를 제외하고, 직접 작성한 메서드는 반드시 Task를 반환하자.)

### Task가 성공하지 못한 경우
- 비동기 작업이 성공하면 좋겠지만 예외가 발생하면 어떻게 될까? 또는 비동기 작업은 실행 중 취소될 수도 있다.
    - .NET에서 실행에 50ms 이상 걸릴 가능성이 있는 메서드의 경우 비동기 메서드로 선언한다.
- Task의 상태는 Canceled, Faulted인 채로 완료되며 await으로 대기한 곳에서 예외를 발생시킨다.

- 정리하자면 await으로 대기한 비동기 작업에서 예외가 발생한 경우, 동기 메서드와 동일하게 try-catch 로 처리하면 된다. 
- 예시에서 예외를 발생시켜보자.
    - Task.Delay는 취소를 지원하고, 범위에 벗어난 인자 전달 시 ArgumentOutOfRangeException 예외를 발생시킨다.

```cs
async Task PrintNumberAsync(CancellationToken token)
{
    try
    {
        await Task.Delay(-1, token);

        Console.WriteLine(5);
    }
    catch (OperationCanceledException)
    {
        // 취소 시 예외 처리 
    }
    catch (ArgumentOutOfRangeException)
    {
        // Task.Delay() 예외 처리
    }
}
```
