﻿using Iamport.RestApi.Models;
using System.Threading.Tasks;

namespace Iamport.RestApi.Apis
{
    /// <summary>
    /// 비인증(비회원) 결제를 수행할 수 있는 API가 구현해야할 인터페이스입니다.
    /// </summary>
    /// <seealso href="http://api.iamport.kr/#!/subscribe.customer"/>
    public interface ISubscribeApi : IIamportApi
    {
        /// <summary>
        /// [POST] /subscribe/customers/{customer_uid}
        /// iamport: 구매자에 대해 빌링키 발급 및 저장
        /// </summary>
        /// <param name="registration">구매자 등록 정보</param>
        /// <returns>구매자 정보</returns>
        Task<Customer> RegisterCustomerAsync(CustomerRegistration registration);
        /// <summary>
        /// [GET] /subscribe/customers/{customer_uid}
        /// iamport: 구매자의 빌링키 정보 조회
        /// </summary>
        /// <param name="customerId">구매자 ID</param>
        /// <returns>구매자 정보</returns>
        Task<Customer> GetCustomerAsync(string customerId);
        /// <summary>
        /// [DELETE] /subscribe/customers/{customer_uid}
        /// iamport: 구매자의 빌링키 정보 삭제(DB에서 빌링키를 삭제[delete] 합니다)
        /// </summary>
        /// <param name="customerId">구매자 ID</param>
        /// <returns>구매자 정보</returns>
        Task<Customer> DeleteCustomerAsync(string customerId);
        /// <summary>
        /// [POST] /subscribe/payments/onetime
        /// iamport: 구매자로부터 별도의 인증과정을 거치지 않고, 카드정보만으로 결제를 진행하는 API입니다(아임포트 javascript가 필요없습니다).
        /// customer_uid를 전달해주시면 결제 후 다음 번 결제를 위해 성공된 결제에 사용된 빌링키를 저장해두게되고, customer_uid가 없는 경우 저장되지 않습니다.
        /// 동일한 merchant_uid는 재사용이 불가능하며 고유한 값을 전달해주셔야 합니다.
        /// </summary>
        /// <param name="request">간편 결제 정보</param>
        /// <returns>결제 결과</returns>
        Task<Payment> DoDirectPaymentAsync(DirectPaymentRequest request);
        /// <summary>
        /// [POST] /subscribe/payments/again
        /// iamport: 저장된 빌링키로 재결제를 하는 경우 사용됩니다. /subscribe/payments/onetime 또는 /subscribe/customers/{customer_uid} 로 등록된 빌링키가 있을 때 매칭되는 customer_uid로 재결제를 진행할 수 있습니다.
        /// </summary>
        /// <param name="request">저장된 구매자 정보로 결제를 요청하는 정보</param>
        /// <returns>결제 결과</returns>
        Task<Payment> DoDirectPaymentAsync(CustomerDirectPaymentRequest request);
        /// <summary>
        /// [POST] /subscribe/payments/schedule
        /// iamport: 비인증 결제요청을 미리 예약해두면 아임포트가 자동으로 해당 시간에 맞춰 결제를 진행하는 방식입니다.(/subscribe/payments/again 를 아임포트가 대신 수행하는 개념)
        ///        결제가 이뤄지고나면 Notification URL로 결제성공/실패 결과를 POST request로 보내드립니다.
        ///
        /// 1. 기존에 빌링키가 등록된 customeruid가 존재하는 경우 해당 customer_uid와 해당되는 빌링키로 schedule정보가 예약됩니다.(카드정보 선택사항)
        /// 2. 등록된 customer_uid가 없는 경우 빌링키 신규 발급을 먼저 진행한 후 schedule정보를 예약합니다.(카드정보 필수사항)
        ///
        /// 예약건 별로 고유의 merchant_uid를 전달해주셔야 합니다.
        ///
        /// schedules의 상세정보(선택정보) buyer_name, buyer_email, buyer_tel, buyer_addr, buyer_postcode는 누락되는 경우에 한해,
        /// customer_uid에 해당되는 customer_name, customer_email, customer_tel, customer_addr, customer_postcode 정보로 대체됩니다.
        /// (buyer 정보는 customer_정보에 우선합니다)
        /// </summary>
        /// <param name="request">스케줄 등록 정보</param>
        /// <returns>등록된 스케줄의 목록</returns>
        Task<ScheduledPayment[]> SchedulePaymentsAsync(SchedulePaymentsRequest request);
        /// <summary>
        /// [POST] /subscribe/payments/unschedule
        /// iamport: 비인증 결제요청예약 취소
        /// </summary>
        /// <param name="request">예약 취소 정보</param>
        /// <returns>취소된 스케줄 목록</returns>
        Task<ScheduledPayment[]> UnschedulePaymentsAsync(UnschedulePaymentsRequest request);
    }
}
