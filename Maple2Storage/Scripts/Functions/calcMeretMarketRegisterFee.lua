function calcMeretMarketRegisterFee(activeListingCount)
    if activeListingCount == 0 then
        return 190
    elseif activeListingCount == 1 then
        return 290
    else
        return 390
    end
end
