---
title: 동시성 프로그래밍
date: 2024-07-27 08:00:00 +09:00
categories: [비동기 프로그래밍]
tags:
  [
    동시성,
    멀티스레드,
    비동기,
    Task,
  ]
---

- 개요
- async await, Task, 비동기 프로그래밍, 병렬 처리, 멀티스레드 등... 알아갈수록 복잡한 개념인 이것들은 동시성 프로그래밍이라는 소프트웨어의 핵심 특성으로 묶여있다. 동시성은 오래전부터 존재했지만, 이를 제대로 구현하는 것은 쉽지 않다. 이는 동시성이 무엇인지 충분히 이해하지 않고, 구현하기 때문이라고 생각한다. 해당 게시물에서는 두 가지를 다룬다. 

1. [개념] 동시성 프로그래밍이 무엇인지
2. [C# 적용] C#에서 동시성 프로그래밍을 올바르게 사용하는 법

### 0. 동시성 프로그래밍 Concurrency
- 동시성 프로그래밍이란 무엇인가? 개념부터 다져보자.

> '동시성'이란 한 번에 두 가지 이상의 작업을 수행함을 의미한다.
- 당연하지만 와닿지 않는 위 문장을 이해하기 위해서 동시성의 하위 개념인 멀티스레드와 비동기 프로그래밍부터 알아보자.

### 1. 멀티스레딩
- 멀티스레딩이란?
    - 다수의 실행 스레드를 사용

- 개발자가 알아야할 것은 '스레드'는 하위 레벨이라는 것이다.
> '스레드'는 하위 레벨.

```
Quiz: '스레드'는 '???'의 추상화다.

코드가 실행된다는 것은 CPU에 의해 연산되는 것이다. 
cpu는 멀티 코어로 이루어지고, 코어는 스레드 실행의 주체가 된다. 코어는 컨텍스트 스위칭하며 다수의 스레드를 실행한다. (Context Switching)
```
- [이미지]

- 설명을 들어도 여전히 우리의 질문은 남아있다.
```
그래서 c#에서는 어떻게 스레드를 사용하면 돼?
```
- 답은 ...
```
스레드를 사용하지 않는다.
```
- 극단적으로 말해서 조금 더 풀어서 설명해보자면,
    - (개발자는 하위 레벨 개념인) 스레드를 (직접) 사용하지 않는다.

- "스레드는 프로세스보다 작다/적은 비용이다."와 같이 스레드 남용에 큰 책임이 있는 해당 문장과 달리, 생각보다 스레드의 절대적 비용은 결코 작지 않다. (하나의 스레드는 약 4kb의 메모리를 사용한다.)

- 비용적인 측면 외에도 관리 문제가 남아 있다. 개발자가 스레드를 사용한다면, 해당 스레드의 생성 및 재사용 또한 직접 해야 한다. 그러나 우리가 원하는 것이 스레드의 관리인가? 아니다. 우리는 작업 관점에서 생각하고 싶다. 우리는 단지 작업을 빠르고 효율적으로 처리하고 싶지, 작업을 어떤 스레드에 할당하고, 해당 스레드를 어떻게 재사용할 지 고민하는 것이 아니다.  

> 이를 위해 C#에서는 보다 *강력한 상위 레벨 추상화로 개발자가 직접 Thread를 사용하지 않도록 한다. 
- *강력한: 효율적이고, 유연해 사용하기 쉬운

- 스레드의 관리(생성 및 재사용)는 C# 컴파일러에게 맡기고, 개발자는 그보다 상위 레벨인 병렬 처리, 프로그래밍에만 집중할 수 있다.
    

- 그럼 이제 상위 레벨 추상화인 병렬 처리를 알아볼 차례다.

### 2. 병렬 프로그래밍 
> 많은 작업을 여러 스레드에 나눠서 동시에 수행
- 병렬 프로그래밍은 우리가 원했던 작업 관점에서 프로그래밍이고, 내부적으론 스레드풀에 의해 최적의 알고리즘으로 처리된다!

```
스레드 풀은 자동으로 작업을 스레드에 할당한다는 개념이다.
다수의 스레드를 생성 및 재사용하여 작업을 처리하는 멀티스레딩 개념이 포함된다. 
```

- C#에서는 작업들을 Task 클래스로 표현하고, 매우 간단하고 유연하게 병렬 처리한다. (우리는 모르지만 내부적으론 최적화된 알고리즘으로 멀티스레딩된다.)

- 간단한 예제를 보자.
```cs
// 작업 1
var task1 = Task.Run(() => {
    // 피보나치 수열 100개 출력
});

// 작업 2
var task2 = Task.Run(() => {
    // 또 다른 수열 100개 출력
});

// 병렬 프로그래밍
await Task.WhenAll(task1, task2);
```

- Task를 사용한 병렬 프로그래밍의 내부 알고리즘 및 우아한 응용 방법은 주제에 벗어나므로, 다른 게시물에서 다루기로 한다.
- 예제추가


- 한숨 쉬었다가, 동시성 프로그래밍의 두 번째 '비동기 프로그래밍'을 알아보자.
- 예시
    - 병렬 처리 -> 버거킹에서 알바생 여러 명
    -> 병렬 프로그래밍이 알바생을 더 '빨리'일하게 하는 것이 아니다. 알바생이 프라이 튀기는 시간을 10초에서 5초로 단축시킬 수 없다. 단, 빅맥세트에서 버거와 프라이를 두 명의 알바생에게 처리하게 하는 것이다.
    -> 물론 알바생 여러 명이 프라이 굽는 시간이 다르면, 여러 명의 알바생에게 작업을 할당하고, 제일 빠른 알바생(7초)한테 받을 수 있다. 
    - 다수의 요청을 여러 '알바생'이 적절하게 처리하는 것.

### 비동기
- 비동기 프로그래밍이란??
    > 불필요한 스레드의 사용을 피하려고 콜백을 사용하는 동시성의 한 형태
    - 전혀 와닿지 않는 정의다. 처음부터 생각해보자.

- async await을 마주하면서 우리는 비동기 프로그래밍이라는 개념을 맞닥드리고, 이를 파고들수록 점점 더 비동기 프로그래밍이 무엇인지 헷갈리게 된다.

> 왜 비동기 프로그래밍을 이해하기 어려울까? 그 이유는 왜 비동기 프로그래밍을 해야하는지 모르기 때문이다.
- 비동기 프로그래밍의 목적과 필요성부터 이해해야 한다.

- 우리가 GUI 앱을 만들어야 한다고 가정하자.
- 좋은 앱이란 무엇인가? 직관적이고, 간단한 UI 를 표시하는 앱일 것이다. 이것이 전부인가? 한 가지가 더 필요하다.
    - 유저의 입력에 빠른 반응성을 가져야 한다. (우리는 모두 느린 반응 속도로 이중 터치를 경험해본적이 있다.)

- 어떤 입력에 대한 처리가 3초가 걸린다고 생각해보자 (멀티스레딩을 하던 어떻게 하던 3초는 걸릴 수 있는 작업). 충분히 크지 않은 시간이지만 만약 3초동안 키오스크가 멈춰있다면??
    - [박살난 키오스크 이미지]

- 위와 같은 문제를 겪지 않기 위해 우리는 스레드를 둘로 나눠야 한다. UI 표시(및 상호작용)을 담당하는 스레드1와 오래 걸리는 작업을 처리하는 스레드2.
    - 스레드1: UI 처리만 담당해 빠른 반응성을 책임져야한다. UI 스레드라 한다.
    - 스레드2: 입력이 들어왔을 때, 연산 작업을 처리하고 그 결과를 UI 스레드에 전달한다. 워커 스레드라고 한다.
    - [스레드1, 2 이미지]

- 이제까지 계속 '스레드' 관점에서 생각하지 말라고 했는데, 위 문제를 해결하려면 '스레드'를 대체하는 컨텍스트 스위칭이 필요하다.
> '스레드' 관점의 '컨텍스트 스위칭'을 고민하지 않도록 상위레벨로 추상화한 개념이 바로 비동기 프로그래밍이다!
- 작업관점으로 표현해보자.
    1. 버튼 클릭 (UI 입력) 
    2. 오래 걸리는 작업(서버 통신, cpu 연산 작업, ...)
    3. 결과 표시 (UI 출력)

- 1, 3 UI 작업과 2번 연산 작업으로 분리할 수 있고, UI 작업 중에 연산 작업은 실행 주체로 위임한다. 
- 비동기 프로그래밍을 사용하면 내부적으로 1, 3번 작업은 UI 스레드, 2번 작업은 워커스레드로 처리된다.

- 개발자는 단순히 C#이 제공하는 async await만 사용하면 된다.

```cs
// 버튼 클릭 이벤트 핸들러
// 사용자 입력이 들어오면 실행된다. (UI 스레드가 실행함.)
async void ButtonClicked(object? sender, arguments e)
{
    // 연산 작업 (워커 스레드가 실행함.)
    var result = await TaskesSomethingLongAsync();

    // 결과 UI에 표시 (UI 스레드가 실행함.)
    Label.Text = result;
}

private async Task<string> TaskesSomethingLongAsync()
{
    // 오래 걸리는 작업 ... 
    return result;
}
```

> 여기까지 봤으면 비동기 프로그래밍이 필요한 경우를 정의해보자.
- UI 스레드와 워커 스레드는 역할이 다를뿐 **본질적으로 똑같은 스레드**다. 다만 여기에서 희소성에 대한 차이가 있다.
    - 빠른 반응성을 위해 UI 스레드는 최소한의 UI 작업만 맡아야 하고, 나머지 모든 작업은 워커 스레드(들)이 처리해줘야 한다. 즉, 여기서는 UI 스레드가 다른 워커 스레드에 비해 훨씬 값진 리소스인 것이다. 

- 다른 스레드로 작업을 위임한다는 것
    = 요청 스레드를 자유롭게 하기 위해, 다른 스레드 생성 비용을 지불
    > 요청 스레드가 더 가치있는(희소한) 상황에서만 의미 있음

### 비동기 프로그래밍이 필요 없는 경우
- 스레드간 희소성의 차이가 없는 경우도 충분히 존재한다. 대표적으로 콘솔 애플리케이션이다.
    - **반응성이 필요없는** 콘솔 애플리케이션에선 오래된 작업을 할 떄는 비동기 프로그래밍을 할 필요가 없다.

- 콘솔 애플리케이션에서 cpu 중심 태스크 하나를 장시간 수행할 때, 스레드를 분리시켜도 별다른 장점이 없다. 오히려 스레드를 나눈다면, 추가 스레드가 작업을 수행할 동안 메인 스레드는 대기할 뿐이다.

- 주의
    - 콘솔 애플리케이션에서 멀티 스레드 자체가 의미 없다는 것이 아니다. cpu 중심 작업을 여러개 실행하는 경우라면 GUI, 콘솔 애플리케이션 모두 멀티스레딩을 사용하는 것이 좋다.

- 참고
    - Concurrency in C# Cookbook


### 결론

# 모어 이펙티브
## 29: 동기, 비동기 메서드를 함께 사용해서는 안된다.
- async 메서드는 작업을 마치기 전 객체(=요청한 작업의 상태)를 반환할 수 있음을 나타낸다.

## 아이템 30: 비동기 메서드를 사용해서 스레드 생성과 콘텍스트 전환을 피하라
- 다른 스레드로 작업을 위임한다는 것 
    = 요청 스레드를 자유롭게 하기 위해, 다른 스레드 생성 비용을 지불
    > 요청 스레드가 더 가치있는(희소한) 상황에서만 의미 있음
        - 예시: GUI 애플리케이션의 메인(=UI)스레드, 사용자는 요청한 작업 완료 전이라도, 사용자의 입력에 UI 스레드가 즉각적으로 반응하기를 바란다.  
    > 반대로 GUI가 아닌 경우, CPU 중심 작업을 비동기로 수행할 필요가 없다.
        - 예시: 콘솔 애플리케이션에서 cpu 중심 태스크 하나를 장시간 수행할 때, 스레드를 분리시켜도 별다른 장점이 없다. 오히려 스레드를 나눈다면, 추가 스레드가 작업을 수행할 동안 메인 스레드는 대기할 뿐이다.
        - 주의: 콘솔 애플리케이션에서 비동기가 의미 없다는 것이 아니다. cpu 중심 작업을 여러개 실행하는 경우라면 GUI, 콘솔 애플리케이션 모두 여러 스레드를 사용하는 것이 좋다.

## 아이템 32: 비동기 작업은 태스크 객체를 사용해 구성하라
- 태스크는 다른 리소스(주로 스레드)에 작업을 위임할 수 있도록 추상화한 개념이다.


